import { environment } from '../environments/environment';
import tonrich from './tonrich';
import cssExports from './main.scss';

class tonscan extends tonrich {
  numberOfElement = 0;
  override addBadge(): void {
    this.addTonrichBadgeToTonScan();
  }

  addTonrichBadgeToTonScan() {
    this.addBadgeElement();

    setInterval(() => {
      var numberOfCurrentElement = document.querySelectorAll('tbody').length;
      if (numberOfCurrentElement != this.numberOfElement) {
        this.numberOfElement = numberOfCurrentElement;
        this.addBadgeElement();
      }
    }, 1000);
  }

  addBadgeElement(): boolean {
    let transactions = document.querySelectorAll('tbody');

    let hasNewTransaction = false;

    for (let i = 0; i < transactions.length; i++) {
      let record = transactions[i] as HTMLElement;

      let tonrichTagElement = record.children[0].children[2] as HTMLElement;
      let walletId = this.getWalletId(record.childNodes[0] as HTMLElement);

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
        this.showSite(
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

  getWalletId(htmlElement: HTMLElement): string | null {
    var walletElement = htmlElement.querySelector('a.address-link');
    if (walletElement) {
      let stringUrl = walletElement.getAttribute('href');
      if (stringUrl) {
        return stringUrl.substring('/address/'.length);
      }
    }

    return null;
  }
}

export default tonscan;