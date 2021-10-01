import Unity, { UnityContext } from "react-unity-webgl";
import "./app.css";
import Header from "./Header";
import Footer from "./Footer";

export const context = new UnityContext({
  loaderUrl: "build/maple-fighters.loader.js",
  dataUrl: "build/maple-fighters.data",
  frameworkUrl: "build/maple-fighters.framework.js",
  codeUrl: "build/maple-fighters.wasm",
});

function App() {
  return (
    <div>
      <Header />
      <Footer />
      <Unity className="container" unityContext={context} />
    </div>
  );
}

export default App;
