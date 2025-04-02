export class LLMSessionManager {
    dotNetRef;

    constructor(dotNetRef) {
        this.dotNetRef = dotNetRef;
    }

    static init(dotNetRef) {
        return new LLMSessionManager(dotNetRef);
    }

    async test() {
        await this.dotNetRef.invokeMethodAsync("Test", 1);
    }

    async isAvailable() {
        if (typeof LanguageModel == "undefined") {
            return "unavailable";
        } else {
            return await LanguageModel.availability();
        }
    }

    async createSession() {
        var availability = await this.isAvailable();

        if (availability == "downloadable") {
            var l = 0, t = 1;

            var instance = this;

            var interval = setInterval(async function () {
                await instance.dotNetRef.invokeMethodAsync("JSDownloadProgress", l, t);

                if (l == t) {
                    clearInterval(interval);

                    return;
                }

                
            }, 1000);

            var session = await LanguageModel.create({
                monitor(m) {
                    m.addEventListener("downloadprogress", e => {
                        l = e.loaded;
                        t = e.total;
                    });
                }
            });



            return new LLMSession(session);
        } else {
            var session = await LanguageModel.create();

            return new LLMSession(session);
        }
    }
}

export class LLMSession {
    #aiSession;

    constructor(aiSession) {
        this.#aiSession = aiSession;
    }

    async prompt(msg) {
        return await this.#aiSession.prompt(msg);
    }
}