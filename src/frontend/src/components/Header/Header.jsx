import { context } from "../App/App.jsx";
import "./header.css";
import logo from "../../assets/logo.png";
import { useState } from "react";

function EnterFullScreen() {
  context.setFullscreen(true);
}

function Login() {
  context.send("Authenticator Controller", "ShowLoginWindow");
}

function Header() {
  let [gameButtons, setGameButtons] = useState(false);

  setTimeout(() => {
    setGameButtons(true);
  }, 2000);

  return (
    <>
      <img src={logo} className="logo" alt="logo" />
      <div className="title">
        <h2>Maple Fighters</h2>
      </div>
      <div>
        {gameButtons && (
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
    </>
  );
}

export default Header;
