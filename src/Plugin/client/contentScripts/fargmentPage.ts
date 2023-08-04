import { environment } from '../environments/environment';
import cssExports from './fragmentPage.scss';

var walletId: string | null = '';
let numberOfElement = 0;
addTonrichBadge();

function addTonrichBadge() {
  if (window.location.host.indexOf('fragment.com') !== -1) {
    addTonrichBadgeToFragment();
  } else if (window.location.host.indexOf('tonviewer.com') !== -1) {
    addTonrichBadgeToTonViewer();
  } else if (window.location.host.indexOf('tonscan.org') !== -1) {
    addTonrichBadgeToTonScan();
  }
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
    if (htmlElement?.classList.contains('tm-wallet')) {
      let stringUrl = htmlElement.getAttribute('href');
      if (stringUrl) {
        let lastIndexOf = stringUrl.lastIndexOf('/');
        return stringUrl.substring(lastIndexOf + 1);
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
    var walletElement = htmlElement.querySelector('a.address-link');
    if (walletElement) {
      let stringUrl = walletElement.getAttribute('href');
      if (stringUrl) {
        return stringUrl.substring('/address/'.length);
      }
    }
  
  return null;
}

function showSite(url: string, element: HTMLElement, style: string) {
  var iframe = document.getElementById('tonapi-iframe');
  if (iframe) {
    iframe.remove();
  }

  var newIframe = document.createElement('iframe') as HTMLIFrameElement;

  newIframe.classList.add(style);

  newIframe.id = 'tonapi-iframe';
  newIframe.src = url;
  newIframe.addEventListener('mouseleave', (e) => {
    var iframe = document.getElementById('tonapi-iframe');
    if (iframe) {
      iframe.remove();
    }
  });

  element.appendChild(newIframe);
}

function addTonrichBadgeToFragment() {
  handleShowMore();
  addBadge();

  function handleShowMore() {
    var showMore = document.querySelector('.table-cell-more') as HTMLElement;

  showMore.addEventListener('click', (e) => {
      let hasNewElment = false;

      let interval = setInterval(() => {
        hasNewElment = addBadge();
        if (hasNewElment) {
          clearInterval(interval);
          setTimeout(() => {
            handleShowMore();
          }, 500);
        }
      }, 100);
    });
  }

  function addBadge(): boolean {
    let transactions = document.querySelectorAll('.tm-wallet');

    let getTable = document.querySelectorAll('.tm-table-wrap');

    for (let i = 0; i < getTable.length; i++) {
      (getTable[i] as HTMLElement).style.overflow = 'unset';
    }

    let setPosition = document.querySelectorAll('.tm-section-bid-info');
    setPosition.forEach((task) => {
      (task as HTMLElement).style.overflow = 'unset';
    });

    let hasNewTransaction = false;
    //todo: add tonrich badge
    for (let i = 0; i < transactions.length; i++) {
      let transaction: HTMLElement = transactions[i] as HTMLElement;

      if (transaction.classList.contains('new-elm-added')) {
        continue;
      }

      hasNewTransaction = true;

      transaction.classList.add('new-elm-added');
      let tonrichTagDiv = document.createElement('div');

      // TonrichIcon.src = `${environment.tonrichAddress}/images/fragmant-icon.svg`;
      tonrichTagDiv.classList.add(cssExports['tonrich-tag-fragment']);
      tonrichTagDiv.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        let currentWalletId = getWalletId(transaction);
        walletId = currentWalletId;
       
        showSite(
          `${environment.tonrichAddress}/${walletId}`,
          transaction,
          cssExports['tonrich-page-fragment']
        );
      });

      let tonrichIcon = document.createElement('img');
      tonrichIcon.src = 'https://tonrich.app/images/fragmant-icon.svg';
      tonrichTagDiv.appendChild(tonrichIcon);

      transaction.insertBefore(tonrichTagDiv, transaction.children[0]);
      transaction.classList.add(cssExports['tonrich-target-element-fragment']);
    }

    return hasNewTransaction;
  }
}

function addTonrichBadgeToTonViewer() {
  setTimeout(() => {
    addBadge();
  }, 1000);

  function addBadge(): boolean {
    let transactions = document.querySelectorAll('.simple');

    let hasNewTransaction = false;

    for (let i = 0; i < transactions.length; i++) {
      let transaction = transactions[i] as any;

      let reactProps = transaction[Object.keys(transaction).filter((k) => k.includes('Props'))[0]];

      if (!reactProps || transaction.classList.contains('new-elm-added')) {
        continue;
      }

      hasNewTransaction = true;

      transaction.classList.add('new-elm-added');
      let tonrichTagDiv = document.createElement('div');

      // TonrichIcon.src = `${environment.tonrichAddress}/images/fragmant-icon.svg`;
      tonrichTagDiv.classList.add(cssExports['tonrich-tag-tonviwer']);
      tonrichTagDiv.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        var currentWalletId = getWalletId(transaction);
        walletId = currentWalletId;
        showSite(
          `${environment.tonrichAddress}/${walletId}`,
          transaction,
          cssExports['tonrich-page-tonviwer']
        );
      });
      let tonrichIcon = document.createElement('img');
      tonrichIcon.src = 'https://tonrich.app/images/fragmant-icon.svg';
      tonrichTagDiv.appendChild(tonrichIcon);
      transaction.insertBefore(tonrichTagDiv, transaction.children[0]);
      transaction.classList.add(cssExports['tonrich-target-element-tonviwer']);
    }

    return hasNewTransaction;
  }

  setInterval(() => {
    var numberOfCurrentElement = document.querySelectorAll('a').length;
    if (numberOfCurrentElement != numberOfElement) {
      numberOfElement = numberOfCurrentElement;
      addBadge();
    }
  }, 1000);
}

function addTonrichBadgeToTonScan() {
  addBadge();

  function addBadge(): boolean {
    let transactions = document.querySelectorAll('tbody');

    let hasNewTransaction = false;

    for (let i = 0; i < transactions.length; i++) {
      let record = transactions[i] as HTMLElement;

      let tonrichTagElement = record.children[0].children[2] as HTMLElement;
      let walletId = getWalletIdFromTonscan(record.childNodes[0] as HTMLElement);
        
      if (!walletId || tonrichTagElement.classList.contains('new-elm-added')) {
        continue;
      }

      hasNewTransaction = true;

      tonrichTagElement.classList.add('new-elm-added');
      let tonrichTagDiv = document.createElement('div');

      tonrichTagDiv.classList.add(cssExports['tonrich-tag-tonscan']);
      tonrichTagDiv.addEventListener('click', async (e) => {
        e.stopPropagation();
        e.preventDefault();
        let tagElem = tonrichTagElement;
        let currentWalletId = walletId;
        console.log('show wallet', currentWalletId);
        showSite(
          `${environment.tonrichAddress}/${currentWalletId}`,
          tagElem,
          cssExports['tonrich-page-tonscan']
        );
      });

      let tonrichIcon = document.createElement('img');
      tonrichIcon.src = 'https://tonrich.app/images/fragmant-icon.svg';

      tonrichTagDiv.appendChild(tonrichIcon);
      tonrichTagElement.insertBefore(tonrichTagDiv, tonrichTagElement.children[0]);
      tonrichTagElement.classList.add(cssExports['tonrich-target-element-tonscan']);
    }

    return hasNewTransaction;
  }

  setInterval(() => {
    var numberOfCurrentElement = document.querySelectorAll('tbody').length;
    if (numberOfCurrentElement != numberOfElement) {
      numberOfElement = numberOfCurrentElement;
      addBadge();
    }
  }, 1000);
}
