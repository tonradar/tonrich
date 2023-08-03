import { environment } from '../environments/environment';
import cssExports from './fragmentPage.scss';

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
    console.log(e);
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
  if (window.location.host.indexOf('fragment.com') !== -1) {
    return getWalletIdFromFragment(htmlElement);
  } else if (window.location.host.indexOf('tonviewer.com') !== -1) {
    return getWalletIdFromTonviewer(htmlElement);
  } else if (window.location.host.indexOf('tonscan.org') !== -1) {
    return getWalletIdFromTonscan(htmlElement);
  }

  return null;
}

function getWalletIdFromFragment(htmlElement: HTMLElement): string | null {
  if (htmlElement) {
    if (htmlElement.classList.contains('table-cell')) {
      var firstElement = htmlElement.firstElementChild;
      if (firstElement?.classList.contains('tm-wallet')) {
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

function getWalletIdFromTonviewer(htmlElement: any): string | null {
  if (htmlElement) {
    let reactProps = htmlElement[Object.keys(htmlElement).filter((k) => k.includes('Props'))[0]];
    if (reactProps?.children?.props?.address) {
      return reactProps?.children?.props?.address.toString() ?? null;
    } else if (!!reactProps?.children && Array.isArray(reactProps?.children)) {
      var wallet = reactProps?.children?.filter(
        (f: { props: { children: { props: { address: any } } } }) => f?.props?.children?.props?.address
      );
      if (wallet && wallet.length > 0) {
        return wallet[0].props.children?.props?.address;
      }
    }
  }

  return null;
}

function getWalletIdFromTonscan(htmlElement: HTMLElement): string | null {
  if (htmlElement.tagName == 'TD') {
    var walletElement = htmlElement.querySelector(':scope > div > a');
    if (walletElement) {
      let stringUrl = walletElement.getAttribute('href');
      if (stringUrl) {
        return stringUrl.substring('/address/'.length);
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

  newIframe.classList.add(cssExports['tonrich-page']);
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
