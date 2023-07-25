import { environment } from "../environments/environment";
import cssExports from "./fragmentPage.scss";

var walletId: string | null = '';

let transactions = document.querySelectorAll('.tm-wallet');

let getTable = document.querySelectorAll(".tm-table-wrap");

for (let i = 0; i <getTable.length ; i++) {
  (getTable[i] as HTMLElement).style.overflow = "unset";
}

let setPosition = document.querySelectorAll(".tm-section-bid-info");
setPosition.forEach(task =>{
  (task as HTMLElement).style.overflow = "unset";
})
debugger;


//todo: add tonrich badge
for (let i = 0; i < transactions.length; i++) {
  let TonrichIcon = document.createElement("img");
  TonrichIcon.src = "https://tonrich.app/images/fragmant-icon.svg";
  // TonrichIcon.src = `${environment.tonrichAddress}/images/fragmant-icon.svg`;
  TonrichIcon.classList.add(cssExports["newElm"]);
  TonrichIcon.addEventListener("mouseover", async (e) => {
    let element = transactions[i] as HTMLElement;
    var currentWalletId = getWalletId(element);
    if (!currentWalletId || currentWalletId === walletId)
      return;

    walletId = currentWalletId;
    console.log(currentWalletId);
    showSite(`${environment.tonrichAddress}/${walletId}`, element);
  });
  (transactions[i] as HTMLElement).insertBefore(TonrichIcon, (transactions[i] as HTMLElement).children[0]);
  (transactions[i] as HTMLElement).style.display = "flex";
  (transactions[i] as HTMLElement).style.alignItems = "center";
  (transactions[i] as HTMLElement).style.justifyContent = "center";
  (transactions[i] as HTMLElement).style.position = "relative";
}

function getWalletId(htmlElement: HTMLElement): string | null {
  let stringUrl = htmlElement.getAttribute('href');
  if (stringUrl) {
    let lastIndexOf = stringUrl.lastIndexOf('/');
    return stringUrl.substring(lastIndexOf + 1);
  }
  return null;
}

function showSite(url: string, element: HTMLElement) {
  var iframe = document.getElementById('tonapi-iframe');
  if (iframe) {
    iframe.remove();
  }

  var newIframe = document.createElement('iframe') as HTMLIFrameElement;

  newIframe.classList.add(cssExports["tonrich-page"]);
  newIframe.id = 'tonapi-iframe';
  newIframe.src = url;
  element.appendChild(newIframe);
}

addEventListener("click", (event) => {
  var iframe = document.getElementById('tonapi-iframe');
  if (!iframe) {
    return;
  }
  let target = event.target;
  if (target != iframe) {
    walletId = null;
    iframe.remove();
  }
});