<template>
  <div>
    <mdb-row>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col class="text-center">
        <mdb-row>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
          <mdb-col> <standard-banner></standard-banner><br /> </mdb-col>
        </mdb-row>
        <br />
        <br />

        <!--
                <p class="font-weight-bold bg-danger text-white w-75 mx-auto">
                    Please do not use PTP to earn for now as there are some errors in counting valid hits for memebers and we will solve that error as soon as possible.
                    If you are advertiser you can still advertise with our ptp system as we already have too many fixed ad spots in many sites that advertise our ptp rotator
                </p>
                    -->
        <p class="font-weight-bold">
          We pay you for 3 levels of referrals deep. <br />
          We pay
          {{ this.$store.getters.faucetSettingValue("PTPLevel1").Value }}% to
          you from all your Level 1 earnings,<br />
          {{ this.$store.getters.faucetSettingValue("PTPLevel2").Value }}% from
          all your level 2 referrals earnings,<br />
          {{ this.$store.getters.faucetSettingValue("PTPLevel3").Value }}% to
          you from all your level 3 referrals earnings
        </p>
        <br />

        <p class="font-weight-bold text-danger">
          Non-trusted sources of traffic like trafficg and autohits sites or
          other rotators<br />
          or any other sources of traffic that generate traffic automatically or
          in iframes are not allowed.
        </p>
        <br />

        <span class="font-weight-bold text-danger"
          >If we find any such sources in your PTP earning records, we will not
          grant your withdrawals.</span
        >
        <br />
        <br />

        <mdb-card class="card text-white bg-secondary w-75 mx-auto">
          <mdb-card-body>
            <strong>Your PTP link is:</strong> {{ ptpURL }}
          </mdb-card-body>
        </mdb-card>
        <br />
      </mdb-col>
      <mdb-col col="1">
        <skyscrapper-banner></skyscrapper-banner>
      </mdb-col>
      <mdb-col col="1"></mdb-col>
    </mdb-row>
    <mdb-row>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
      <mdb-col>
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
        <standard-banner></standard-banner><br />
      </mdb-col>
    </mdb-row>

    <button
      type="button"
      class="bottomLeftButton"
      id="bottomLeftButton"
      onClick="document.getElementById('bottomLeftBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomLeftBanner" id="bottomLeftBanner">
      <square-banner></square-banner>
    </div>

    <button
      type="button"
      class="bottomRightButton"
      id="bottomRightButton"
      onClick="document.getElementById('bottomRightBanner').remove();this.remove();"
    >
      x
    </button>
    <div class="bottomRightBanner" id="bottomRightBanner">
      <square-banner></square-banner>
    </div>
  </div>
</template>

<script>
import {
  mdbRow,
  mdbCol,
  mdbBtn,
  mdbIcon,
  mdbInput,
  mdbCard,
  mdbCardHeader,
  mdbCardBody,
  mdbCardTitle,
  mdbCardText
} from "mdbvue";

export default {
  components: {
    mdbRow,
    mdbCol,
    mdbBtn,
    mdbIcon,
    mdbInput,
    mdbCard,
    mdbCardHeader,
    mdbCardBody,
    mdbCardTitle,
    mdbCardText
  },
  data() {
    return {
      recaptchav2SiteKey: process.env.VUE_APP_KEY_Recaptchav2,
      recaptchav3SiteKey: process.env.VUE_APP_KEY_Recaptchav3,
      address: this.$store.state.userData[0].FaucetHubBitcoinAddress,
      apiResponse: null,
      ptpURL: `http://localhost:8080/#/?referrer=${
        this.$store.state.userData[0].Username
      }`,
      errorList: null,
      successList: null
    };
  },
  methods: {
    test: function() {}
  },
  mounted() {}
};
</script>
