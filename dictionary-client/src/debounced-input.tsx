import { useRef, useState } from "react";

type Props = React.HTMLProps<HTMLTextAreaElement> & {
    debounceMs: number;
    submit: (text: string) => void;
};

function DebouncedInput({ debounceMs, submit, ...inputProps }: Props) {
    const [input, setInput] = useState("");
    const timeoutRef = useRef<number | null>(null);

    const _onChange = (text: string) => {
        setInput(text);

        if (timeoutRef.current) clearTimeout(timeoutRef.current);
        timeoutRef.current = setTimeout(() => submit(text), debounceMs);
    };

    return <textarea value={input} onChange={(e) => _onChange(e.target.value)} {...inputProps} />;
}

export default DebouncedInput;
