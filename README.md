# cmd-coffee-csharp-core
An interactive CLI for interacting with [cmd.coffee](https://cmd.coffee) written in csharp-core.

<img src="https://images.unsplash.com/photo-1564676676973-89fef0028fad" alt="Photo by Michael C on Unsplash" width="70%"/>

## Project Health Status

See [Git Action](https://github.com/cmd-coffee/cmd-coffee-csharp-core/actions) for status

## Getting Started

> Right now the only way to install this client is by building from source. You'll need to have [dotnet-core 3.0](https://dotnet.microsoft.com/download/dotnet-core/3.0) installed to do so.

```sh
> git clone git@github.com:cmd-coffee/cmd-coffee-cshare-core.git
> cd cmd-coffee-csharp-core/src
> dotnet build --configuration Release
```

### Usage

```sh
> ./CmdCoffee.Cli/bin/Release/netcoreapp3.0/cmdcoffee.cli

cmd.coffee> help

| Command                       | Description                                                                      |
|------------------------------------------------------------------------------------------------------------------|
| products [product-code]       | list available coffees for order. specify product-code to see additional details |
| buy product-code [promo-code] | place an order for one of our products                                           |
| orders [order-key]            | list your orders. specify order-key to see additional details                    |
| init invite-code              | request an access-key                                                            |
```

## Contributing

1. Fork it (<https://github.com/your-github-user/cmd-coffee-csharp-core/fork>)
2. Create your feature branch (`git checkout -b my-new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin my-new-feature`)
5. Create a new Pull Request
