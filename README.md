# 飞行课程上课登记系统

[![MIT licensed](https://img.shields.io/badge/license-MIT-blue.svg)](https://gitlab.aiursoft.cn/anduin/flyclass/-/blob/master/LICENSE)
[![Pipeline stat](https://gitlab.aiursoft.cn/anduin/flyclass/badges/master/pipeline.svg)](https://gitlab.aiursoft.cn/anduin/flyclass/-/pipelines)
[![Test Coverage](https://gitlab.aiursoft.cn/anduin/flyclass/badges/master/coverage.svg)](https://gitlab.aiursoft.cn/anduin/flyclass/-/pipelines)
[![ManHours](https://manhours.aiursoft.cn/r/gitlab.aiursoft.cn/anduin/flyclass.svg)](https://gitlab.aiursoft.cn/anduin/flyclass/-/commits/master?ref_type=heads)
[![Website](https://img.shields.io/website?url=https%3A%2F%2Fflyclass.aiursoft.cn)](https://flyclass.aiursoft.cn)

## Try

Try a running FlyClass [here](https://flyclass.aiursoft.cn).

## Run in Ubuntu

The following script will install\update this app on your Ubuntu server. Supports Ubuntu 22.04.

On your Ubuntu server, run the following command:

```bash
curl -sL https://gitlab.aiursoft.cn/anduin/flyclass/-/raw/master/install.sh | sudo bash
```

Of course you can append a custom port number to the command:

```bash
curl -sL https://gitlab.aiursoft.cn/anduin/flyclass/-/raw/master/install.sh | sudo bash -s 8080
```

It will install the app as a systemd service, and start it automatically. Binary files will be located at `/opt/apps`. Service files will be located at `/etc/systemd/system`.

## Run locally

Requirements about how to run

1. [.NET 7 SDK](http://dot.net/)
2. Execute `dotnet run` to run the app
3. Use your browser to view [http://localhost:5000](http://localhost:5000)

## Run in Microsoft Visual Studio

1. Open the `.sln` file in the project path.
2. Press `F5`.

## How to contribute

There are many ways to contribute to the project: logging bugs, submitting pull requests, reporting issues, and creating suggestions.

Even if you with push rights on the repository, you should create a personal fork and create feature branches there when you need them. This keeps the main repository clean and your workflow cruft out of sight.

We're also interested in your feedback on the future of this project. You can submit a suggestion or feature request through the issue tracker. To make this process more effective, we're asking that these include more information to help define them more clearly.
