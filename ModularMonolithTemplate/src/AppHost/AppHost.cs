using Microsoft.Extensions.Hosting;
using Aspire.Hosting;
using Aspire.Hosting.MySql;

var builder = DistributedApplication.CreateBuilder(args);

// ---------- Datastores ----------
var redis = builder.AddRedis("redis")
    .WithDataVolume("redisdata")
    .WithRedisCommander(); // opcional, UI web p/ Redis

var rabbit = builder.AddContainer("rabbitmq", "rabbitmq:4-management")
    .WithEnvironment("RABBITMQ_DEFAULT_USER", "guest")
    .WithEnvironment("RABBITMQ_DEFAULT_PASS", "guest")
    .WithHttpEndpoint(port: 15672, targetPort: 15672, name: "ui") // http://localhost:15672
    .WithEndpoint(name: "amqp", port: 5672, targetPort: 5672);

// MySQL database using Aspire component
var mysql = builder.AddMySql("mysql");
var mysqlWriteDb = mysql.AddDatabase("billing-write");
var mysqlReadDb = mysql.AddDatabase("billing-read");

// ---------- Observability: Tempo (traces), Loki (logs), Collector, Grafana ----------
var tempo = builder.AddContainer("tempo", "grafana/tempo:2.8.2")
    .WithBindMount("../observability/tempo.yaml", "/etc/tempo.yaml")
    .WithArgs("-config.file=/etc/tempo.yaml", "-target=all")
    .WithHttpEndpoint(port: 3200, targetPort: 3200)
    .WithEndpoint(name: "otlp-grpc", port: 4317, targetPort: 4317)
    .WithEndpoint(name: "otlp-http", port: 4318, targetPort: 4318);

var loki = builder.AddContainer("loki", "grafana/loki:3.5.5")
    .WithBindMount("../observability/loki-config.yaml", "/etc/loki/config.yml")
    .WithArgs("-config.file=/etc/loki/config.yml", "-config.expand-env=true")
    .WithHttpEndpoint(port: 3100, targetPort: 3100);

var otel = builder.AddContainer("otel-collector", "otel/opentelemetry-collector-contrib:0.135.0")
    .WithBindMount("../observability/otelcol.yaml", "/etc/otelcol.yaml")
    .WithArgs("--config=/etc/otelcol.yaml")
    // Expose non-default host ports to avoid conflicts with other host listeners
    .WithEndpoint(name: "otlp-grpc", port: 14317, targetPort: 4317)
    .WithEndpoint(name: "otlp-http", port: 14318, targetPort: 4318)
    .WithHttpEndpoint(port: 8889, targetPort: 8889)
    .WaitFor(loki)
    .WaitFor(tempo)
    ;

var grafana = builder.AddContainer("grafana", "grafana/grafana:12.1.1")
    .WithBindMount("../observability/grafana/provisioning", "/etc/grafana/provisioning")
    .WithHttpEndpoint(port: 3000, targetPort: 3000)
    .WithEnvironment("GF_AUTH_ANONYMOUS_ENABLED", "true")
    .WithEnvironment("GF_AUTH_ANONYMOUS_ORG_ROLE", "Admin")
    ;

// Prometheus to scrape metrics exposed by the OTEL Collector
var prometheus = builder.AddContainer("prometheus", "prom/prometheus:v2.55.0")
    .WithBindMount("../observability/prometheus.yaml", "/etc/prometheus/prometheus.yml")
    .WithHttpEndpoint(port: 9090, targetPort: 9090);

// ---------- Seu app ----------
// Referencia ao projeto de API (nome gerado por Aspire: Projects.Api). Ajuste se seu projeto tiver outro nome.
builder.AddProject<Projects.Api>("webapp")
    .WithReference(redis)
    // Provide connection strings to the API with well-known names
    .WithReference(mysqlWriteDb, "BillingWrite")
    .WithReference(mysqlReadDb, "BillingRead")
    // Ensure API starts after MySQL databases are ready
    .WaitFor(mysqlWriteDb)
    .WaitFor(mysqlReadDb)
    // Enable structured logs to Loki via Serilog sink
    .WithEnvironment("LOG_LOKI_ENABLED", "true")
    .WithEnvironment("LOG_LOKI_URI", "http://localhost:3100")
    .WithEnvironment("Rabbit__Connection", "amqp://guest:guest@rabbitmq:5672")
    // Exportador OTLP (traces/logs/metrics) → Collector (use localhost for host-running project)
    .WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:14317")
    .WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "grpc")
    // Atributos úteis
    .WithEnvironment("OTEL_RESOURCE_ATTRIBUTES",
        "service.name=webapp,deployment.environment=Development");


builder.Build().Run();
