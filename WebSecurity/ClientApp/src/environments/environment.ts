import version from "./version.json"

export const environment = {
  production: false,
  baseServiceUrl: "https://localhost:65400",
  apiVersion: version.apiversion,
  version: version.version,
  serverName: "Hydra Enterprise Server",
  webSocketUrl: "ws://localhost:65400/ws"
};
