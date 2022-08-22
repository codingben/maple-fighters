import "./mobile.css";
import logo from "../../assets/logo.png";

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

export default Mobile;
