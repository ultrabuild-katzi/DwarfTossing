{
  pkgs,
  lib,
  config,
  inputs,
  ...
}: {
  languages.dotnet = {
    enable = true;
    package = pkgs.dotnet-sdk_8;
  };

  # required tools for DOTNET PublishAot
  packages = with pkgs; [
    zlib
    gcc
  ];

  enterShell = ''
    export LIBRARY_PATH="${pkgs.zlib}/lib:$LIBRARY_PATH"
    export LD_LIBRARY_PATH="${pkgs.zlib}/lib:$LD_LIBRARY_PATH"
    
    touch .devenv/.gdignore
    touch .direnv/.gdignore
    touch .idea/.gdignore
  '';
}

