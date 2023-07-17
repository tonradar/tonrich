import { environment } from "../environments/environment";
import cssExports from "./fragmentPage.scss";

var walletId: string | null = '';

let transactions = document.querySelectorAll('.tm-wallet');
//todo: add tonrich badge
for (let i = 0; i < transactions.length; i++) {
  (transactions[i] as HTMLElement).style.boxShadow = "0 0 10px rgba(255,255,255,.5)";
  (transactions[i] as HTMLElement).style.padding = "2px";
  (transactions[i] as HTMLElement).style.borderRadius = "5px";
  (transactions[i] as HTMLElement).classList.add(cssExports["blink"]);
}

addEventListener('mouseover', async (e) => {
  var element = (e as any).fromElement as HTMLElement;

  var currentWalletId = getWalletId(element);

  if (currentWalletId && currentWalletId != walletId) {
    walletId = currentWalletId;
    console.log(currentWalletId);
    showSite(`${environment.tonrichAddress}/${walletId}`, element);
  } else {
    if (element.classList.contains("tm-wallet") || element.parentElement?.classList.contains("tm-wallet")) {
      return;
    }

    var iframe = document.getElementById('tonapi-iframe');
    if (iframe) {
      iframe.remove();
      walletId = null;
      currentWalletId = null;
    }
  }
});




function getWalletId(htmlElement: HTMLElement): string | null {


  if(window.location.host.indexOf("fragment.com") !== -1){
    return getWalletIdFromFragment(htmlElement);
  }
  else if(window.location.host.indexOf("tonviewer.com") !== -1){
    return getWalletIdFromTonviewer(htmlElement);
  }else if(window.location.host.indexOf("tonscan.org") !== -1){
    return getWalletIdFromTonscan(htmlElement);
  }


  return null;
}

function getWalletIdFromFragment(htmlElement: HTMLElement): string | null {
  if (htmlElement) {
    if (htmlElement.classList.contains("table-cell")) {
      var firstElement = htmlElement.firstElementChild;
      if (firstElement?.classList.contains("tm-wallet")) {
        let stringUrl = firstElement.getAttribute('href');
        if (stringUrl) {
          let lastIndexOf = stringUrl.lastIndexOf('/');
          return stringUrl.substring(lastIndexOf + 1);
        }
      }
    }
  }

  return null;
}


function getWalletIdFromTonviewer(htmlElement: HTMLElement): string | null {
  if (htmlElement) {
  var walletElement =  htmlElement.querySelector(":scope > span>div>div.address-wrapper-container>span")
    if (walletElement) {
    
      console.log(walletElement.innerHTML.toString());
      
      return walletElement.innerHTML?.toString() ?? null;
    }
  }

  return null;
}


function getWalletIdFromTonscan(htmlElement: HTMLElement): string | null {
  if (htmlElement) {
    if (htmlElement.classList.contains("table-cell")) {
      var firstElement = htmlElement.firstElementChild;
      if (firstElement?.classList.contains("tm-wallet")) {
        let stringUrl = firstElement.getAttribute('href');
        if (stringUrl) {
          let lastIndexOf = stringUrl.lastIndexOf('/');
          return stringUrl.substring(lastIndexOf + 1);
        }
      }
    }
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

