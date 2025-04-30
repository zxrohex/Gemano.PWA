Object.extend = function (destination, source) {
    for (var property in source) {
        if (source.hasOwnProperty(property)) {
            destination[property] = source[property];
        }
    }

    return destination;
};

Object.extend(LanguageModel, {
    getInputQuota: function () {
        return this.inputQuota;
    },
    getInputUsage: function () {
        return this.inputUsage;
    }
});