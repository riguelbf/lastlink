# ModularMonolithTemplate

This repository is configured to use Husky.Net (dotnet tool) to run Git hooks that help maintain code quality.

- pre-commit: build and linter (format verify)
- pre-push: build, tests, linter, and optional CodeRabbit CLI

## Prerequisites

- .NET SDK installed
- Git installed and initialized in this folder

## One-time setup

```bash
# Restore local dotnet tools (Husky.Net, dotnet-format)
dotnet tool restore

# Initialize Git repo if not already initialized
# (Required so Husky can wire hooks into .git/hooks)
git init

# Install Husky.Net Git hooks (creates .husky/_ and installs hooks)
dotnet husky install

# Ensure hook files are executable (macOS/Linux)
chmod +x .husky/pre-commit .husky/pre-push
```

## Hooks behavior

### pre-commit
- Builds the solution: `dotnet build ModularMonolithTemplate.sln --configuration Release --nologo`
- Verifies formatting without modifying files: `dotnet format --verify-no-changes`
  - If issues are reported, run `dotnet format` to fix them, stage the changes, and try again.

### pre-push
- Builds the solution: `dotnet build`
- Runs tests: `dotnet test --no-build`
- Verifies formatting: `dotnet format --verify-no-changes`
- Optionally runs CodeRabbit CLI if installed locally: `coderabbit --plain`
  - Non-strict by default (does not block push if CLI returns non-zero).
  - To enforce blocking behavior, export `CODERABBIT_STRICT=1` before pushing.

## Optional: enable CodeRabbit CLI locally

Get AI code reviews locally before pushing your changes.

1) Install CLI

```bash
curl -fsSL https://cli.coderabbit.ai/install.sh | sh
# Then restart your shell or reload your rc file, e.g.:
# source ~/.zshrc
```

2) Authenticate

```bash
coderabbit auth login
# or
cr auth login
```

3) Try a local review manually

```bash
coderabbit --plain
# to compare against a base branch
coderabbit --base develop --plain
```

4) Integrate with pre-push hook

The pre-push hook auto-detects the `coderabbit` command. If present, it runs:

```bash
coderabbit --plain
```

- To make CodeRabbit failures block your push, enable strict mode:

```bash
export CODERABBIT_STRICT=1
# then push as usual
```

## Skipping hooks when needed

- Skip commit hook: `git commit -m "message" --no-verify`
- Skip push hook: `git push --no-verify`

Use sparingly and preferably only in emergency situations.

## Notes

- Husky.Net and dotnet-format are defined in `.config/dotnet-tools.json` and restored via `dotnet tool restore`.
- Hook scripts are in `.husky/`.
