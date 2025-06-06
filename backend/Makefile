MIGRATION_NAME ?= DefaultMigration
PROJECT=Infrastructure
STARTUP=Presentation
MIGRATIONS_DIR=DataBase/Migrations
ENVIRONMENT=Development

# ex: make migrate MIGRATION_NAME=CreateUsersTable

migrate:
	sh ./Infrastructure/scripts/ef-migration.sh $(MIGRATION_NAME) \
		--project $(PROJECT) \
		--output-dir $(MIGRATIONS_DIR) \
		--environment $(ENVIRONMENT)

# ex: make db-update
db-update:
	sh ./Infrastructure/scripts/ef-update.sh \
		--project $(PROJECT) \
		--environment $(ENVIRONMENT)

# ex: make remove-migrations
remove-migrations:
	dotnet ef migrations remove \
		--project $(PROJECT) \
		-- --environment $(ENVIRONMENT)

# Start development containers
docker-up:
	docker-compose -f docker-compose.yml up -d

# Stop and remove development containers
docker-down:
	docker-compose -f docker-compose.yml down

# View container logs
docker-logs:
	docker-compose -f docker-compose.yml logs -f