import Unity, { UnityContext } from "react-unity-webgl";
import { isMobile } from "react-device-detect";
import Mobile from "../Mobile/Mobile.jsx";
import Header from "../Header/Header.jsx";
import Footer from "../Footer/Footer.jsx";
import Loader from "../Loader/Loader.jsx";
import "./app.css";

export const context = new UnityContext({
  loaderUrl: "files/WebGL.loader.js",
  dataUrl: "files/WebGL.data",
  frameworkUrl: "files/WebGL.framework.js",
  codeUrl: "files/WebGL.wasm",
});

function OnSetEnv() {
  // Timeout is required because of the splash screen,
  // so it takes a few seconds to load game scene.
  setTimeout(() => {
    let isProduction = process.env.REACT_APP_ENV == "Production";
    let env = isProduction ? "Production" : "Development";

    context.send("Environment Setter", "SetEnvCallback", env);
  }, 2000);
}

function App() {
  if (isMobile) {
    return <Mobile />;
  }

  context.on("SetEnv", OnSetEnv);

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
