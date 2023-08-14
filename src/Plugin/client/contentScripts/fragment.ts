import { environment } from '../environments/environment';
import tonrich from './tonrich';
import cssExports from './main.scss';

class fragment extends tonrich {
  walletId: string | null = '';
  numberOfElement = 0;

  override addBadge(): void {
    this.addTonrichBadgeToFragment();
  }

  addTonrichBadgeToFragment() {
    this.handleShowMore();
    this.addBadgeElement();
  }

  handleShowMore() {
    var showMore = document.querySelector('.table-cell-more') as HTMLElement;

    if (showMore) {
      showMore.addEventListener('click', (e) => {
        let hasNewElment = false;

        let interval = setInterval(() => {
          hasNewElment = this.addBadgeElement();
          if (hasNewElment) {
            clearInterval(interval);
            setTimeout(() => {
              this.handleShowMore();
            }, 500);
          }
        }, 100);
      });
    }
  }

  addBadgeElement(): boolean {
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
      tonrichTagDiv.addEventListener('click', (e) => {
        e.stopPropagation();
        e.preventDefault();
        let currentWalletId = this.getWalletId(transaction);

        this.showSite(
          `${environment.tonrichAddress}/${currentWalletId}`,
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

  getWalletId(htmlElement: HTMLElement): string | null {
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
}

export default fragment;
