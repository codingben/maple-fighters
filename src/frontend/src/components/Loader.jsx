import { useState } from "react";
import { context } from "./App";
import FadeLoader from "react-spinners/FadeLoader";
import "./loader.css";

function Loader() {
  let [loading, setLoading] = useState(true);

  context.on("loaded", () => {
    setLoading(false);
  });

  return (
    <div className="loader">
      <FadeLoader
        css={"display: block;"}
        size={50}
        color={"white"}
        loading={loading}
        speedMultiplier={1.5}
      />
    </div>
  );
}

export default Loader;
