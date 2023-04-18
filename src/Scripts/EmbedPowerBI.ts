import * as pbi from 'powerbi-client';
declare var powerbi: pbi.service.Service;

$(document).ready(function () {
    new ReportLoader();
});


const displaySettings = {
    navContentPaneEnabled: false,
    filterPaneEnabled: false,
    dataPointsEnabled: false, // ?
    panes: {
        filters: {
            visible: false
        },
        visualizations: {
            visible: false
        },
        pageNavigation: {
            visible: false
        },
    },
    bars: {
        actionBar: {
            visible: false
        }
    }
};


export class EmbedInfo implements pbi.models.IReportEmbedConfiguration {
    accessToken: string;
    embedUrl:    string;
    id:          string;
    type:        string                  = 'report';
    tokenType:   pbi.models.TokenType    = pbi.models.TokenType.Embed;
    permissions: pbi.models.Permissions  = pbi.models.Permissions.All;
    viewMode:    pbi.models.ViewMode = pbi.models.ViewMode.Edit;
    settings:    pbi.models.ISettings = displaySettings;
    

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

        self.getEmbedInfoFromPage()
            .then(data => self.embedReport(data));
    }

    private embedReport(embedInfo: EmbedInfo) {
        let $reportContainer = $('#reportContainer')[0];
        powerbi.embed($reportContainer, embedInfo);
    }

    private async getEmbedInfoFromPage() {

        let _embedConfig = $('#embedconfig');
        if (_embedConfig[0]) {
            let embedInfo = new EmbedInfo({
                "accessToken": _embedConfig[0].dataset.embedtoken,
                "embedUrl"   : _embedConfig[0].dataset.embedurl,
                "id"         : _embedConfig[0].dataset.reportid
            });

            return embedInfo; 
        }
        else {
            const error = new Error('failed to get embed config data from content template page')
            return Promise.reject(error)
        }
    }


    // disused - get the embed info from our controller endpoint via fetch
    //private async getEmbedInfoAsync() {
    //    const response = await fetch('/PowerBIEmbed');
    //    const data = await response.json();

    //    if (response.ok) {
    //        return new EmbedInfo(data);  // << partial object into constructor
    //    }
    //    else {
    //        const error = new Error('failed to make fetch call to retrieve embed data')
    //        return Promise.reject(error)
    //    }
    //}
}
