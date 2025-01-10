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

export class GemanoSession {
    #aiSession;
    messages;
    id;
    name;

    constructor(session) {
        this.#aiSession = session;

        this.messages = [];

        this.id = GemanoHelpers.generateRandomId();

        this.name = "New Chat Session";
    }



    #addMessage(msgRole, msgContent) {
        this.messages.push({ role: msgRole, content: msgContent });
    }

    async prompt(msg) {
        this.#addMessage("user", msg);

        var response = await this.#aiSession.prompt(msg);

        this.#addMessage("llm", response);

        this.save();

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

    getId() {
        return this.id;
    }

    getName() {
        return this.name;
    }

    setName(name) {
        this.name = name;

        this.save();
    }

    getMessages() {
        return this.messages;
    }

    save() {
        GemanoChatConversationManager.save(new GemanoChatConversation(this.id, this.name, this.messages));
    }

    static async getStatus() {
        if (typeof ai == "undefined") {
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


export class GemanoChatConversation {
    id;
    name;
    messages;

    constructor(id, name, messages) {
        this.id = id;
        this.name = name;
        this.messages = messages;
    }

    getId() {
        return this.id;
    }

    getName() {
        return this.name;
    }

    getMessages() {
        return this.messages;
    }
}

export class GemanoChatConversationManager {
    static save(conversation) {
        var conversations = this.getAll();

       

        if (this.getChat(conversation.id) != null) {
            conversations[conversations.findIndex(c => c.id == conversation.id)] = conversation;
        }
        else {
            conversations.push(conversation);
        }

        localStorage.setItem("chats", JSON.stringify(conversations));
    }

    static getAll() {
        var rawChats = localStorage.getItem("chats");
        
        if (rawChats == null) {
            localStorage.setItem("chats", JSON.stringify([]));
        }

        try {
            var conversations = JSON.parse(rawChats);       
        } catch (e) {
            localStorage.setItem("chats", JSON.stringify([]));

            conversations = [];
        }

        
        return conversations.map(chat => {
            return new GemanoChatConversation(chat.id, chat.name, chat.messages);
        });
    }

    static getChat(id) {
        var conversations = this.getAll();

        console.dir(conversations);

        console.dir(conversations.find(chat => chat.id == id));

        return conversations.find(chat => chat.id == id);
    }
}