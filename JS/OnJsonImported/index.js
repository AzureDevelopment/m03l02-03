const converter = require('json-2-csv');

module.exports = function (context, myBlob) {
    context.log(
        'JavaScript blob trigger function processed blob \n Blob:',
        context.bindingData.blobTrigger,
        '\n Blob Size:',
        myBlob.length,
        'Bytes'
    );
    const json = JSON.parse(myBlob.toString());
    const arrayOfUsers = Object.keys(json).map((key) => {
        return json[key];
    });
    converter.json2csv([arrayOfUsers], (error, csv) => {
        context.bindings.outputBlob = csv;
        context.done();
    });
};
