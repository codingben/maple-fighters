import Unity, { UnityContext } from "react-unity-webgl";
import { isMobile } from "react-device-detect";
import FadeLoader from "react-spinners/FadeLoader";
import "./app.css";
import logo from "../../assets/logo.png";
import { useState } from "react";

const context = new UnityContext({
  loaderUrl: "files/example-app.loader.js",
  dataUrl: "files/example-app.data",
  frameworkUrl: "files/example-app.framework.js",
  codeUrl: "files/example-app.wasm",
});

function EnterFullScreen() {
  context.setFullscreen(true);
}

function Login() {
  context.send("Authenticator Controller", "ShowLoginWindow");
}

function OnSetEnv() {
  // Timeout is required because of the splash screen,
  // so it takes a few seconds to load game scene.
  setTimeout(() => {
    let isProduction = process.env.REACT_APP_ENV == "Production";
    let env = isProduction ? "Production" : "Development";

    context.send("Environment Setter", "SetEnvCallback", env);
  }, 2000);
}

function Mobile() {
  return (
    <div className="warning">
      <img src={logo} alt="logo" />
      <h1>
        <a href="https://github.com/codingben/maple-fighters">Maple Fighters</a>{" "}
        is not compatible with your device.
      </h1>
    </div>
  );
}

function App() {
  if (isMobile) {
    return <Mobile />;
  }

  let [loading, setLoading] = useState(true);

  context.on("loaded", () => {
    setLoading(false);
  });

  context.on("SetEnv", OnSetEnv);

  return (
    <div>
      <div>
        <img src={logo} className="logo" alt="logo" />
        <div className="title">
          <h2>Maple Fighters</h2>
        </div>
        <div>
          {loading == false && (
            <div>
              <button onClick={EnterFullScreen} className="fullscreen-button">
                Enter Full Screen
              </button>
              <button onClick={Login} className="login-button">
                Login
              </button>
            </div>
          )}
          <a href="https://github.com/codingben/maple-fighters" target="_blank">
            <button className="github-button">GitHub</button>
          </a>
        </div>
      </div>
      <Unity className="container" unityContext={context} />
      <div className="footer">
        <h4>
          Made with <span style={{ color: "#E91E63" }}>&#x2764;</span> by{" "}
          <a href="https://codingben.io" target="_blank">
            Ben Oukhanov
          </a>
        </h4>
      </div>
      <div className="loader">
        <FadeLoader
          css={"display: block;"}
          size={50}
          color={"white"}
          loading={loading}
          speedMultiplier={1.5}
        />
      </div>
    </div>
  );
}

export default App;
