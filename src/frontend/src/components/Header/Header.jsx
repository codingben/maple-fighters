import { context } from "./App";
import "./header.css";
import logo from "../assets/logo.png";

function EnterFullScreen() {
  context.setFullscreen(true);
}

function Header() {
  return (
    <>
      <img src={logo} className="logo" alt="logo" />
      <div className="title">
        <h2>Maple Fighters</h2>
      </div>
      <div>
        <button onClick={EnterFullScreen} className="fullscreen-button">
          Enter Full Screen
        </button>
        <a href="https://github.com/benukhanov/maple-fighters" target="_blank">
          <button className="github-button">GitHub</button>
        </a>
      </div>
    </>
  );
}

export default Header;
