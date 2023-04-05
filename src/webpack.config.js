const path = require('path');
const webpack = require("webpack");


module.exports = {
    entry: {
        embedpowerbi: ['./Scripts/EmbedPowerBI.ts']
    },
    output: {
        filename: '[name].bundle.js',
        libraryTarget: 'var',
        library: '[name]',
        // path: path.resolve(__dirname, 'wwwroot/js'),
        path: path.resolve(__dirname, 'sitefiles/s1/themes/custom1/wwwroot/js'),
        publicPath: '/dist/'
    },
    module: {
        rules: [                
            {
                test: /\.tsx?$/,
                loader: 'ts-loader',
                exclude: /node_modules/
            }
        ]
    },
    resolve: {
        extensions: [".tsx", ".ts", ".js"]
    },
    devtool: 'source-map'
};