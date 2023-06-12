

// module.exports = {
//   mode: 'production',
//   entry: {
//     background: 'client/background.ts',
//     fargmentPage: 'client/contentScripts/fargmentPage.ts'
//   },
//   optimization: {
//     runtimeChunk: false,
//   },
//   plugins: [],
// } as Configuration;

import {Configuration} from 'webpack';


import * as path from 'path';
import * as CopyPlugin from 'copy-webpack-plugin';

const config: Configuration = {
  entry: {
    background: './client/background.ts',
    fargmentPage: './client/contentScripts/fargmentPage.ts'
  },
  module: {
    rules: [
      {
        test: /\.ts?$/,
        use: 'ts-loader',
        exclude: /node_modules/,
      },
      { test: /\.scss$/, use: [ 
        { loader: "style-loader" },  // to inject the result into the DOM as a style block
        { loader: "css-modules-typescript-loader"},  // to generate a .d.ts module next to the .scss file (also requires a declaration.d.ts with "declare modules '*.scss';" in it to tell TypeScript that "import styles from './styles.scss';" means to load the module "./styles.scss.d.td")
        { loader: "css-loader", options: { modules: true } },  // to convert the resulting CSS to Javascript to be bundled (modules:true to rename CSS classes in output to cryptic identifiers, except if wrapped in a :global(...) pseudo class)
        { loader: "sass-loader" },  // to convert SASS to CSS
        // NOTE: The first build after adding/removing/renaming CSS classes fails, since the newly generated .d.ts typescript module is picked up only later
    ] }
    ],
  },
  resolve: {
    extensions: ['.ts', '.js'],
  },
  output: {
    path: path.resolve(__dirname, 'dist'),
  },
  plugins:[
    new CopyPlugin({
      patterns: [
        { from: "client/manifest.json", to: "manifest.json" },
        { from: "client/assets", to :"assets"},
        { from: "client/index.html"}
      ],
    }),
  ]
  
};

export default config;