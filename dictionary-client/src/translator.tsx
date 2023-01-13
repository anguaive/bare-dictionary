import { useState } from "react";
import DebouncedInput from "./debounced-input";
import styles from "./translator.module.css";
import axios from "axios";
import translateSvg from "../assets/translate.svg";
import { translate as translateApi } from "./api";

function Translator() {
    const [input, setInput] = useState("");
    const [result, setResult] = useState<string | null>("");
    const [loading, setLoading] = useState(false);

    const submit = async (text: string) => {
        setInput(text);
        if (text.length) {
            if (!translateApi) {
                console.error("Translate API is not available! (VITE_API variable is not set)");
                return;
            }

            setLoading(true);
            try {
                const res = await axios.get(translateApi, { params: { text } });
                console.log(res);
                setResult(res.data);
            } catch (ex) {
                setResult(null);
            }
            setLoading(false);
        } else {
            setResult("");
        }
    };

    let displayedResult = "";
    if (input.length) {
        displayedResult = result === null ? "No translation found" : result;
    }

    return (
        <div className={styles.container}>
            <div className={styles.header}>
                <img src={translateSvg} />
                <span>English -&gt; Hungarian</span>
            </div>
            <div className={styles.input}>
                <DebouncedInput
                    debounceMs={400}
                    submit={(text: string) => submit(text)}
                    placeholder={"Type here..."}
                    autoFocus
                    spellCheck={false}
                />
            </div>
            <div className={styles.result}>
                <span className={result === null ? styles.noTranslation : ""}>
                    {displayedResult}
                </span>
                {loading && <div className={styles.loadingIndicator}>Loading...</div>}
            </div>
        </div>
    );
}

export default Translator;
