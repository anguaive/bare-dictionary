const api: string = import.meta.env.VITE_API;

export const translate = api.length ? api + "/api/translate" : null;
