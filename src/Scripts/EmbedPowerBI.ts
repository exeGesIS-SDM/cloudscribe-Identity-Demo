import * as pbi from 'powerbi-client';
declare var powerbi: pbi.service.Service;

// Work in progress - jk 

class EmbedInfo {
    accessToken: string;
    embedUrl: string;
    id: string;
    type: string = 'report';
    tokenType: pbi.models.TokenType = pbi.models.TokenType.Embed;
    permissions: pbi.models.Permissions = pbi.models.Permissions.All;
    viewMode: pbi.models.ViewMode = pbi.models.ViewMode.Edit;
}


export async function getEmbedInfo() {
    const res = await fetch('/PowerBIEmbed');
    const { data }: { data: EmbedInfo } = await res.json();
    return data;
}


let embedInfo = getEmbedInfo();

let conf = async () => await getEmbedInfo();


// powerbi.embed(document.querySelector('#report-container'), conf);



//export class Fetcher {
//    public static fetchToken(): Promise<string> {
//        return new Promise<string>(
//            function (resolve, reject) {
//                $.getJSON("api/identity/get", function (response) {
//                    resolve(response);
//                }).fail(function () {
//                    resolve(null);
//                });
//            }
//        );
//    }
//}


//class EmbedConfig extends EmbedInfo {
//    type: string = 'report';
//    tokenType: pbi.models.TokenType = pbi.models.TokenType.Embed;
//    permissions: pbi.models.Permissions = pbi.models.Permissions.All;
//    viewMode: pbi.models.ViewMode = pbi.models.ViewMode.Edit;
//}
