import { environment } from "../environments/environment";
import cssExports from "./fragmentPage.scss";

var walletId: string | null = '';

let transactions = document.querySelectorAll('.tm-wallet');

let getTable = document.querySelectorAll(".tm-table-wrap");

for (let i = 0; i < getTable.length; i++) {
  (getTable[i] as HTMLElement).style.overflow = "unset";
}

let setPosition = document.querySelectorAll(".tm-section-bid-info");
setPosition.forEach(task => {
  (task as HTMLElement).style.overflow = "unset";
})

//todo: add tonrich badge
for (let i = 0; i < transactions.length; i++) {
  let transaction: HTMLElement = (transactions[i] as HTMLElement);

  if (transaction.classList.contains("new-elm-added")) {
    continue;
  }

  transaction.classList.add("new-elm-added");
  let TonrichIcon = document.createElement("img");
  TonrichIcon.src = "https://tonrich.app/images/fragmant-icon.svg";
  // TonrichIcon.src = `${environment.tonrichAddress}/images/fragmant-icon.svg`;
  TonrichIcon.classList.add(cssExports["newElm"]);
  TonrichIcon.addEventListener("mouseover", async (e) => {
    var currentWalletId = getWalletId(transaction);
    if (!currentWalletId || currentWalletId === walletId)
      return;

    walletId = currentWalletId;
    console.log(currentWalletId);
    showSite(`${environment.tonrichAddress}/${walletId}`, transaction);
  });
  transaction.insertBefore(TonrichIcon, transaction.children[0]);
  transaction.style.display = "flex";
  transaction.style.alignItems = "center";
  transaction.style.justifyContent = "center";
  transaction.style.position = "relative";
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