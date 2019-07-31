<template>
  <container fluid>
    <p>Direct traffic</p>
  </container>
</template>

<script>
import {
  Card,
  CardBody,
  Column,
  Row,
  Container,
  CardHeader,
  CardText,
  Btn,
  CardTitle
} from "mdbvue";

export default {
  components: {
    Card,
    CardBody,
    Column,
    Row,
    Container,
    CardHeader,
    CardText,
    Btn,
    CardTitle
  },
  data() {
    return {};
  },
  sockets: {
    receivingDirectLink: function(data) {
      this.$socket.emit(
        "claimingDirectCredit",
        JSON.stringify({
          username: this.$store.state.Username,
          ip: this.$store.state.ip,
          valid: this.$store.state.Valid
        })
      );
      window.location.href = data[0].Link;
    }
  },
  mounted() {
    this.$socket.emit(
      "requestingDirectLink",
      JSON.stringify({
        absoluteReferrer: this.$store.state.AbsoluteReferrer,
        referrer: this.$store.state.Referrer,
        username: this.$store.state.Username
      })
    );
  }
};
</script>
