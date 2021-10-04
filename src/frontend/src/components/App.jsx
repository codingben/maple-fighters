import Unity, { UnityContext } from "react-unity-webgl";
import { isBrowser } from "react-device-detect";
import Header from "./Header";
import Footer from "./Footer";
import Loader from "./Loader";
import "./app.css";

export const context = new UnityContext({
  loaderUrl: "build/maple-fighters.loader.js",
  dataUrl: "build/maple-fighters.data",
  frameworkUrl: "build/maple-fighters.framework.js",
  codeUrl: "build/maple-fighters.wasm",
});

function App() {
  if (!isBrowser) {
    return (
      <div className="center">
        <h1>This game is not compatible with your device.</h1>
      </div>
    );
  }

  return (
    <div>
      <Header />
      <Footer />
      <Unity className="container" unityContext={context} />
      <Loader />
    </div>
  );
}

export default App;
