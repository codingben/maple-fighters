import { StrictMode } from "react";
import { render } from "react-dom";
import App from "./app";
import "./index.css";

const element = document.getElementById("root");

render(
  <StrictMode>
    <App />
  </StrictMode>,
  element
);
