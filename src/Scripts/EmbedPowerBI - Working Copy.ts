import * as pbi from 'powerbi-client';
declare var powerbi: pbi.service.Service;

$(document).ready(function () {
    new ReportLoader();
});


export class EmbedInfo implements pbi.models.IReportEmbedConfiguration {
    accessToken: string;
    embedUrl: string;
    id: string;
    type: string                        = 'report';
    tokenType: pbi.models.TokenType     = pbi.models.TokenType.Embed;
    permissions: pbi.models.Permissions = pbi.models.Permissions.All;
    viewMode: pbi.models.ViewMode       = pbi.models.ViewMode.Edit;

    // only 3 of above properties come back from the API
    // so a partial constructor lets the remainder get set to defaults
    public constructor(init?: Partial<EmbedInfo>) {
        Object.assign(this, init);
    }
}

export class ReportLoader {

    private initialised = false;

    constructor() {
        this.init();
    }

    private init = (): void => {
        if (!this.initialised) {
            this.loadReport();
            this.initialised = true;
        }
    }

    private loadReport = (): void => {
        let self = this;

        //self.getEmbedInfo()
        //    .then((data: EmbedInfo) => {
        //        alert(data.embedUrl)
        //        self.embedReport(data)
        //    })
        //    .catch((error) => {
        //        console.log(error)
        //    })

        self.getEmbedInfoAsync()
            .then(data => self.embedReport(data));
    }


   
    //private getEmbedInfo() {

    //    let embedInfo = new EmbedInfo();

    //    $.ajax({
    //        url: '/PowerBIEmbed',
    //        // jsonpCallback: 'callback',
    //        contentType: 'application/javascript',
    //        dataType: "json",
    //        success: function (json) {
    //            embedInfo.accessToken = json.accessToken;
    //            embedInfo.embedUrl = json.embedUrl;
    //            embedInfo.id = json.reportId;
    //        },
    //        error: function () {
    //            alert("Error");
    //        }
    //    });

    //    return embedInfo;
    //}


    private getEmbedInfo() {
        return new Promise((resolve, reject) => {
            $.ajax({
                url: '/PowerBIEmbed',
                // jsonpCallback: 'callback',
                contentType: 'application/javascript',
                dataType: "json",
                success: function (json) {
                    let embedInfo = new EmbedInfo();
                    embedInfo.accessToken = json.accessToken;
                    embedInfo.embedUrl = json.embedUrl;
                    embedInfo.id = json.reportId;
                    resolve(embedInfo);
                },
                error: function (error) {
                    reject(error);
                }
            });
        });
    }

    private embedReport(embedInfo: EmbedInfo) {
        let $reportContainer = $('#reportContainer')[0];
        powerbi.embed($reportContainer, embedInfo);
    }

    // using async fetch call
    private async getEmbedInfoAsync() {
        const response = await fetch('/PowerBIEmbed');
        const data = await response.json();

        if (response.ok) {
            return new EmbedInfo(data);  // << partial object into constructor
        }
        else {
            // handle the errors
            const error = new Error('failed to make fetch call to retrieve embed data')
            return Promise.reject(error)
        }
    }
}
