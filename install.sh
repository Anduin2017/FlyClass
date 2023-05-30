aiur() { arg="$( cut -d ' ' -f 2- <<< "$@" )" && curl -sL https://gitlab.aiursoft.cn/aiursoft/aiurscript/-/raw/master/$1.sh | sudo bash -s $arg; }
flyclass_path="/opt/apps/FlyClassApp"

install_flyclass()
{
    port=$(aiur network/get_port) && echo "Using internal port: $port"
    aiur network/enable_bbr
    aiur install/caddy
    aiur install/dotnet
    aiur git/clone_to https://gitlab.aiursoft.com/aiursoft/flyclass ./FlyClass
    aiur dotnet/publish $flyclass_path ./FlyClass/src/FlyClass.csproj
    aiur services/register_aspnet_service "flyclass" $port $flyclass_path "FlyClass"
    aiur caddy/add_proxy $1 $port

    echo "Successfully installed FlyClass as a service in your machine! Please open $1 to try it now!"
    rm ./FlyClass -rf
}

# Example: install_flyclass http://flyclass.local
install_flyclass "$@"