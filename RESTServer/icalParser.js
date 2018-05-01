let express = require('express');
let ical = require('ical');

let app = express();

let numberOfItemPerPage = 4;

let createRequestUrl = (id) => {
    return "http://ade.polytech.u-psud.fr:8080/jsp/custom/modules/plannings/anonymous_cal.jsp?resources=" + id +"&projectId=3&calType=ical&nbWeeks=52";
}

let objectAsArray = (object) => {
    let data = [];
    for(let property in object) {
        data.push(object[property]);
    }
    data.sort(function(a, b) {
        return a.start - b.start;
    });
    return data;
}

let getInformationForIdAndPage = function(id, cb){
    ical.fromURL(createRequestUrl(id), {}, (err, data) => {
        cb(objectAsArray(data));
    });
}

app.get('/info/:id', (req, res) => {
    let id = req.params.id;
    let page = req.query.page === undefined ? 0 : Number.parseInt(req.query.page);

    
    console.log("Get info from ical with resource id = " + id + " and page = " + page);

    getInformationForIdAndPage(id, (data) => {
        data = data.slice(page * numberOfItemPerPage, page*numberOfItemPerPage + numberOfItemPerPage);
        res.status(200);
        res.send(data);
    });
});

let server = require('http').Server(app);

server.listen(1234);
