module.exports = {
  module: {
    rules: [{
      "test": /\.*/,
      "exclude": /node_modules/,
      "use": [{
        loader: "./conditionalCompilationLoader"
      }]
    }]
  }
};