// This is a partial webpack config, the rest is handled by Angular
module.exports = {
  module: {
    rules: [
      {
        test: /\.g(raph)?ql$/,
        exclude: /node_modules/,
        loader: 'graphql-tag/loader'
      }
    ]
  }
}
