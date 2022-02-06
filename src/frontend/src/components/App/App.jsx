import Unity, { UnityContext } from "react-unity-webgl";
import { isMobile } from "react-device-detect";
import Header from "../Header/Header.jsx";
import Footer from "../Footer/Footer.jsx";
import Loader from "../Loader/Loader.jsx";
import logo from "../../assets/logo.png";
import "./app.css";

export const context = new UnityContext({
  loaderUrl: "files/WebGL.loader.js",
  dataUrl: "files/WebGL.data",
  frameworkUrl: "files/WebGL.framework.js",
  codeUrl: "files/WebGL.wasm",
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

  context.on("SetEnv", () => {
    // Timeout required (maybe because of the splash screen).
    setTimeout(() => {
      let isProduction = process.env.REACT_APP_ENV == "Production";
      let env = isProduction ? "Production" : "Development";

      context.send("Environment Setter", "SetEnvCallback", env);
    }, 2000);
  });

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
