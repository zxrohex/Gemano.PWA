/*
    



*/

export class GemanoSession {
    #aiSession;
    messages;

    constructor(session) {
        this.#aiSession = session;

        this.messages = [];
    }

    #addMessage(msgRole, msgContent) {
        this.messages.push({ role: msgRole, content: msgContent });
    }

    async prompt(msg) {
        this.#addMessage("user", msg);

        var response = await this.#aiSession.prompt(msg);

        this.#addMessage("llm", response);

        return response;
    }

    tokensLeft() {
        return this.#aiSession.tokensLeft;
    }

    tokensSoFar() {
        return this.#aiSession.tokensSoFar;
    }

    maxTokens() {
        return this.#aiSession.maxTokens;
    }

    destroy() {
        this.#aiSession.destroy();

        this.#aiSession = null;
    }

    static async getStatus() {
        if (ai == "undefined") {
            return "undefined";
        } else {
            return (await ai.languageModel.capabilities()).available;
        }
    }

    static async createSession() {
        if (await this.getStatus() != "readily") {
            throw new Error("Not ready");
        } else {
            return new GemanoSession(await ai.languageModel.create());
        }
    }
}
