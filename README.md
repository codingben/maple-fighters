<h1 align="center">Maple Fighters</h1>
<p align="center">
  <img src="docs/images/maplestory-icon.png" width="100px" height="100px"/>
  <br><i>A small online game similar to MapleStory</i><br>
</p>
<p align="center">
  <a href="http://maplefighters.io"><strong>maplefighters.io</strong></a>
  <br>
</p>

## About

[![Unity Build](https://github.com/codingben/maple-fighters/actions/workflows/unity-build.yml/badge.svg)](https://github.com/codingben/maple-fighters/actions/workflows/unity-build.yml)
[![Frontend Build](https://github.com/codingben/maple-fighters/actions/workflows/frontend-build.yml/badge.svg)](https://github.com/codingben/maple-fighters/actions/workflows/frontend-build.yml)
[![Game Service Build](https://github.com/codingben/maple-fighters/actions/workflows/game-service-build.yml/badge.svg)](https://github.com/codingben/maple-fighters/actions/workflows/game-service-build.yml)

Maple Fighters is an online multiplayer game inspired by MapleStory where you battle monsters with others in real-time.

Please **★ Star** if you like it. Made With :heart: For Open Source Community!

## Play Online

Maple Fighters is available at [maplefighters.io](http://maplefighters.io). This is a web game, no installation required. Supported in any web browser with internet connection. Small, optimized, and incredibly fast! 🚀

## Screenshots

| Lobby                             | The Dark Forest                             |
| --------------------------------- | ------------------------------------------- |
| <img src="docs/images/lobby.png"> | <img src="docs/images/the-dark-forest.png"> |

## Technology

**Game Engine**: Unity (_2020.3.17_)  
**Client**: C#, React.js (_C# is compiled to C++ and finally to WebAssembly_)  
**Server**: C# (_.NET 5.0_)  
**Reverse Proxy**: Nginx  
**Cloud**: DigitalOcean  

## Quickstart

### Docker

> 💡 Please make sure you have Docker installed.

1. Clone repository:

```bash
git clone https://github.com/codingben/maple-fighters.git
cd maple-fighters
```

2. Build and run docker images:

```bash
docker compose up
```

### Kubernetes

> 💡 Please make sure you have Kubernetes cluster.

1. Create Kubernetes resources in `maple-fighters` namespace:

```bash
kubectl apply -f https://raw.githubusercontent.com/codingben/maple-fighters/main/release/kubernetes-manifests.yaml
namespace/maple-fighters created
service/frontend-external created
service/game-service created
deployment.apps/frontend created
deployment.apps/game-service created
```

2. Make sure all pods are running:

```bash
kubectl get pods -n maple-fighters
NAME                            READY   STATUS    RESTARTS   AGE
frontend-79d44b9fbb-gf45k       1/1     Running   0          10s
game-service-54f66cbcbb-q9vtb   1/1     Running   0          10s
```

3. Use `EXTERNAL_IP` to access Maple Fighters in a web browser:

```bash
kubectl get service frontend-external -n maple-fighters
NAME                TYPE           CLUSTER-IP      EXTERNAL-IP   PORT(S)        AGE
frontend-external   LoadBalancer   10.101.21.120   <pending>     80:31765/TCP   10s
```

## Contributing

Please read the [contributing guidelines](CONTRIBUTING.md).

## Artwork

The artwork is owned by Nexon Co., Ltd and will never be used commercially.

## License

[AGPL](https://choosealicense.com/licenses/agpl-3.0/)
