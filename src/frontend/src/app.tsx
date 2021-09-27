import Unity, { UnityContext } from "react-unity-webgl";
import "./app.css";

const context = new UnityContext({
  loaderUrl: "build/maple-fighters.loader.js",
  dataUrl: "build/maple-fighters.data",
  frameworkUrl: "build/maple-fighters.framework.js",
  codeUrl: "build/maple-fighters.wasm",
});

function App() {
  return <Unity className="container" unityContext={context} />;
}

export default App;
