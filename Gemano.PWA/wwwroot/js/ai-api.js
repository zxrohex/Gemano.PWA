/*
    



*/

export class GemanoHelpers {
    static generateRandomId() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            const r = Math.random() * 16 | 0, v = c === 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
}

export class AIServicesManager {
    #dotNetRef;
    LLM;

    constructor(dotNetRef) {
        this.#dotNetRef = dotNetRef;

        this.LLM = new AILLMManager(dotNetRef);
    }

    static init(dotNetRef) {
        return new AIServicesManager(dotNetRef);
    }
}

export class AILLMManager {
    #dotNetRef;

    constructor(dotNetRef) {
        this.#dotNetRef = dotNetRef;
    }
    
    async isAvailable() {
        await this.#dotNetRef.invokeMethodAsync("test");

        if (typeof LanguageModel == "undefined") {
            return "unsupported";
        } else {
            return await LanguageModel.availability();
        }
    }

    async create() {
        var languageModel;

        var availability = await this.isAvailable();
        
        if (availability == "downloadable") {
            languageModel = await LanguageModel.create({
                monitor(m) {
                    m.addEventListener("downloadprogress", (e) => {
                        this.dotNetRef.invokeMethodAsync("OnDownloadProgressEvent", e.loaded, e.total);
                    });
                },
            });
        } else if (availability == "available") {
            languageModel = await LanguageModel.create();
        }
    }
 
}

export class AILanguageModelSession {
    #aiSession;

    constructor(aiSession) {
        this.#aiSession = aiSession;
    }
}