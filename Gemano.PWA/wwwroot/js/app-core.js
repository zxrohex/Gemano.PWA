

/*Object.extend = function (destination, source) {
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
    Object.extend(LanguageModel.prototype, {
        getInputQuota: function () {
            return this.inputQuota;
        },
        getInputUsage: function () {
            return this.inputUsage;
        }
    });
}*/

