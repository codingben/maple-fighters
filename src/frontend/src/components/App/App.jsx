import Unity, { UnityContext } from "react-unity-webgl";
import { useState } from "react";
import { isMobile } from "react-device-detect";
import FadeLoader from "react-spinners/FadeLoader";
import "./app.css";
import logo from "../../assets/logo.png";

const unityContext = new UnityContext({
  loaderUrl: "files/WebGL.loader.js",
  dataUrl: "files/WebGL.data",
  frameworkUrl: "files/WebGL.framework.js",
  codeUrl: "files/WebGL.wasm",
});

function EnterFullScreen() {
  unityContext.setFullscreen(true);
}

function ShowLoginWindow() {
  unityContext.send("Authenticator Controller", "ShowLoginWindow");
}

function SetEnvironment() {
  // Timeout is required because of the splash screen,
  // so it takes a few seconds to load game scene.
  setTimeout(() => {
    let isProduction = process.env.REACT_APP_ENV == "Production";
    let env = isProduction ? "Production" : "Development";

    unityContext.send("Environment Setter", "SetEnvCallback", env);
  }, 2000);
}

function MobileApp() {
  return (
    <div className="warning">
      <img src={logo} alt="logo" />
      <h1>
        <a href="https://github.com/codingben/maple-fighters">Maple Fighters</a>{" "}
        is not compatible with your device. Please use a desktop or laptop
        computer.
      </h1>
    </div>
  );
}

function Logo() {
  return (
    <div>
      <img src={logo} className="logo" alt="logo" />
      <div className="title">
        <h2>Maple Fighters</h2>
      </div>
    </div>
  );
}

function FullScreenButton() {
  return (
    <div>
      <button onClick={EnterFullScreen} className="fullscreen-button">
        Enter Full Screen
      </button>
    </div>
  );
}

function LoginButton() {
  return (
    <div>
      <button onClick={ShowLoginWindow} className="login-button">
        Login
      </button>
    </div>
  );
}

function GitHubButton() {
  return (
    <div>
      <a href="https://github.com/codingben/maple-fighters" target="_blank">
        <button className="github-button">GitHub</button>
      </a>
    </div>
  );
}

function Footer() {
  return (
    <div className="footer">
      <h4>
        Made with <span style={{ color: "#E91E63" }}>&#x2764;</span> by{" "}
        <a href="https://codingben.io" target="_blank">
          Ben Oukhanov
        </a>
      </h4>
    </div>
  );
}

function App() {
  if (isMobile) {
    return <MobileApp />;
  }

  const [loading, setLoading] = useState(true);

  unityContext.on("loaded", () => setLoading(false));
  unityContext.on("SetEnv", SetEnvironment);

  return (
    <div>
      <div>
        <Logo />
        <div>
          {loading == false && (
            <div>
              <FullScreenButton />
              <LoginButton />
            </div>
          )}
          <GitHubButton />
        </div>
      </div>
      <div>
        <Unity className="container" unityContext={unityContext} />
      </div>
      <Footer />
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
