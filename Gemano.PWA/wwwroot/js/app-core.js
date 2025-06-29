

window.gemanoJs = {
    llm: {

        getInputTypes: function (multiModal = true) {
            var allInputTypes = [
                { type: "text" },
                { type: "image" },
                { type: "audio" }
            ];

            return multiModal ? allInputTypes : [allInputTypes[0]];
        },
        checkSupport: function () {
            if (window.LanguageModel === undefined) {
                return "unsupported";
            }
        },

        checkAvailability: async function (multiModal = true) {
            if (window.LanguageModel === undefined) {
                return Promise.resolve("unsupported");
            }

            if (window.LanguageModel.availability) {
                var result = await window.LanguageModel.availability({
                    expectedInputs: this.getInputTypes(multiModal)
                });

                return result;
            } else {
                return Promise.resolve("unsupported");
            }
        },
        isReadilyAvailable: async function () {
            var isAvailable = await this.checkAvailability(true);

            if (isAvailable != "unsupported" || isAvailable != "downloading" || isAvailable != "unavailable") {
                return true;
            } else { return false; }
        },
        downloadProgress: -1,
        create: async function (multiModal = true) {
            if (window.LanguageModel === undefined) {
                return Promise.reject("unsupported");
            }
            if (window.LanguageModel.create) {
                var result = await this.checkAvailability(multiModal);

                if (result == "available") {
                    var model = await window.LanguageModel.create({
                        expectedInputs: this.getInputTypes(multiModal)
                    });

                    return model;
                } else if (result == "downloadable") {
                    var ref = this;

                    var model = await window.LanguageModel.create({
                        expectedInputs: this.getInputTypes(multiModal),
                        monitor(m) {
                            m.addEventListener("downloadprogress", e => {
                                ref.downloadProgress = e.loaded;
                            });
                        }
                    });

                    return model;
                }


                
            } else {
                return Promise.reject("unsupported");
            }

        }
    }
}



Object.extend = function (destination, source) {
    for (var property in source) {
        if (source.hasOwnProperty(property)) {
            destination[property] = source[property];
        }
    }

    return destination;
};

if (window.LanguageModel === undefined) {
    window.LanguageModel = {
        availability: function () {
            return Promise.resolve("unsupported");
        }
    }
} else {
    Object.extend(LanguageModel, {
        
        
    });
}

