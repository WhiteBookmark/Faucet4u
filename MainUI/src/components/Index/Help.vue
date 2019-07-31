<template>
  <mdb-container fluid class="text-left">
    <h2 title="Frequently Asked Questions" ref="faqModal" class="overflow-auto">
      Frequently Asked Questions
    </h2>

    <h3>What is Faucet4all?</h3>
    <p>
      Faucet4all is a next-gen Bitcoin faucet. Users earn free Bitcoin by
      interacting and completing tasks. The only requirement is that you have a
      Bitcoin, FaucetHub or DOGE address to send payments to. We will also be
      adding new payout options in the near future!
    </p>

    <h3>What is a faucet?</h3>

    <p>
      Bitcoin-faucets or any cryptocurrency faucet-pay users a few satoshis for
      loading webpages with ads on them.
    </p>

    <h3>When do I get paid?</h3>

    <p>
      You can withdraw as soon as you have at least 20,000 Satoshi or 0.00020000
      worth of Bitcoin(s).
    </p>

    <h3>What do I have to do to trigger a withdrawal?</h3>

    <p>
      Go to the "Withdrawal" page in your dashboard after logging in and create
      a new withdrawal, after your withdrawal has been submitted you can view
      the status of it on the "Withdrawal History" page.
    </p>

    <h3>How long does it take to get my payment?</h3>

    <p>
      That will sometimes depend on the Bitcoin network, but most payouts are
      processed in 24-48 hours (or even sooner). For instant withdrawals you can
      create a FaucetHub withdrawal.
    </p>

    <h3>Do you charge transaction fees for withdrawals?</h3>

    <p>
      Yes. For Bitcoin withdrawals we only charge the fee required to pay the
      miners. For other methods we only charge what the fee is need for
      exchanging via Bitcoin to your preferred wallet.
    </p>

    <h3>What are offerwalls?</h3>

    <p>
      Offerwalls are services that pay users for doing some kind of task. Those
      tasks include watching a video and the ads that come with them; visiting
      web pages; filling out surveys; installing apps; signing up for services,
      etc.
    </p>

    <h3>How does Faucet4all make money?</h3>

    <p>
      We make money from the ads we show, and we get a cut from the offerwalls.
    </p>

    <h3>Do you have a referral program?</h3>

    <p>
      Yes! We pay for referrals up to 3 Levels for life for faucet, offerwalls
      and PTP earnings. You can view all of the details in your dashboard panel.
    </p>

    <h3>Where do I find my referral link?</h3>

    <p>
      You can find your referral link in your "Banners" page which is available
      in your dashboard after you login.
    </p>

    <h3>Where do I find my PTP link?</h3>

    <p>
      You can find your PTP link in your "PTP" page which is available in your
      dashboard after you login.
    </p>

    <h3>I have lost/wish to reset my password.</h3>

    <p>
      Go to the login page and click on the link that says "Forgot Password" in
      the login box.
    </p>

    <h3>Can you reverse a payments that have already been sent?</h3>

    <p>
      Bitcoin payments are irreversible. Once we send a payment to the Bitcoin
      address you supplied, there is no way to recall it. You are responsible
      for making sure the Bitcoin address you supply is one you control. You can
      check your Bitcoin address on your Profile page.
    </p>
  </mdb-container>
</template>

<script>
import {
  mdbCol,
  mdbRow,
  mdbContainer,
  mdbBtn,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText
} from "mdbvue";

export default {
  components: {
    mdbCol,
    mdbRow,
    mdbContainer,
    mdbBtn,
    mdbCard,
    mdbCardHeader,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText
  },
  methods: {
    test: function() {},
    register: function() {
      this.$router.push("AccountRegister");
    }
  },
  data() {
    return {};
  },
  mounted() {}
};
</script>
