# CQRS-EventSourcing
CQRS &amp; Event Sourcing  with Kfka, RabbitMQ, MongoDB and .NET6

## Prerequisites

```bash
Docker (For Kafka, Kafka Drop, RabbitMQ, Sql Server)
Asp.Net 6
Visual Studio 2022 or Rider or Visual Studio Code

```

## Usage

```bash
Open the project location

docker-compose up
```

## Debugging Your Computer

```sh
Change Banking.Account.Command.Api > appsettings > BrokerType section (Kafka or RabbitMQ)

Open project Visual Studio or Rider or Visual Studio Code and run Multiple Project 
(Banking.Account.Command.Api, Banking.Account.Query.Api, Banking.Account.Consumer.Kafka or Banking.Account.Consumer.RabbitMQ)


```