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
    LanguageModel;

    constructor(dotNetRef) {
        this.#dotNetRef = dotNetRef;

        this.LanguageModel = new AILanguageModelManager(dotNetRef);
    }

    static init(dotNetRef) {
        return new AIServicesManager(dotNetRef);
    }
}

export class AILanguageModelManager {
    #dotNetRef;

    constructor(dotNetRef) {
        this.#dotNetRef = dotNetRef;
    }
    
    async isAvailable() {
        await this.#dotNetRef.invokeMethodAsync("test");

        if (typeof ai == "undefined") {
            return "unsupported";
        } else {
            return await ai.languageModel.availability();
        }
    }
}

export class AILanguageModelSession {
    
}