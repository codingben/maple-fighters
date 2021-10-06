import Unity, { UnityContext } from "react-unity-webgl";
import { isMobile } from "react-device-detect";
import Header from "./Header";
import Footer from "./Footer";
import Loader from "./Loader";
import logo from "../assets/logo.png";
import "./app.css";

export const context = new UnityContext({
  loaderUrl: "build/maple-fighters.loader.js",
  dataUrl: "build/maple-fighters.data",
  frameworkUrl: "build/maple-fighters.framework.js",
  codeUrl: "build/maple-fighters.wasm",
});

function App() {
  if (isMobile) {
    return (
      <div className="warning">
        <img src={logo} alt="logo" />
        <h1>
          <a href="https://github.com/benukhanov/maple-fighters">
            Maple Fighters
          </a>{" "}
          is not compatible with your device.
        </h1>
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
