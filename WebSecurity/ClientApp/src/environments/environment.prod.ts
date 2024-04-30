import { version, apiversion } from "./version.json"

export const environment = {
  production: true,
  baseServiceUrl: "https://localhost:65400",
  apiVersion: apiversion,
  version: version,
  serverName: "Hydra Enterprise Server",
  webSocketUrl: "ws://localhost:65400/ws"
};
