import Unity, { UnityContext } from "react-unity-webgl";
import { isMobile } from "react-device-detect";
import Header from "./Header";
import Footer from "./Footer";
import Loader from "./Loader";
import logo from "../assets/logo.png";
import "./app.css";

const buildPath = "build/maple-fighters";

export const context = new UnityContext({
  loaderUrl: buildPath + ".loader.js",
  dataUrl: buildPath + ".data",
  frameworkUrl: buildPath + ".framework.js",
  codeUrl: buildPath + ".wasm",
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

  context.on("SetConfig", () => {
    // Timeout required (maybe because of the splash screen).
    setTimeout(() => {
      let config = JSON.stringify({
        DevHost: "localhost",
        Host: "maplefighters.io",
        Environment:
          process.env.REACT_APP_ENV == "Production"
            ? "Production"
            : "Development",
      });

      context.send("Network Configuration Setter", "SetConfigCallback", config);
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
