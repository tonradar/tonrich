import { environment } from '../environments/environment';
import cssExports from './fragmentPage.scss';

var walletId: string | null = '';

let transactions = document.querySelectorAll('.tm-wallet');
for (let i = 0; i < transactions.length; i++) {
  (transactions[i] as HTMLElement).style.boxShadow = '0 0 10px rgba(255,255,255,.5)';
  (transactions[i] as HTMLElement).style.padding = '2px';
  (transactions[i] as HTMLElement).style.borderRadius = '5px';
  (transactions[i] as HTMLElement).classList.add(cssExports['blink']);
}

addEventListener('mouseover', async (e) => {
  var element = (e as any).fromElement as HTMLElement;
  var currentWalletId = getWalletId(element);
  
  if (currentWalletId && currentWalletId != walletId) {
    walletId = currentWalletId;
    showSite(`${environment.tonrichAddress}/${walletId}`, element);
  } else {
    if (window.location.host.indexOf('fragment.com') !== -1) {
      if (
        element?.classList?.contains('tm-wallet') ||
        element?.parentElement?.classList?.contains('tm-wallet')
      ) {
        return;
      }
    } else if (window.location.host.indexOf('tonviewer.com') !== -1) {
      if (Object.values(element).length == 0) {
        return;
      }
    } else if (window.location.host.indexOf('tonscan.org') !== -1) {
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
  while (!element.contains(newIframe)) {
    element.appendChild(newIframe);
  }
}
